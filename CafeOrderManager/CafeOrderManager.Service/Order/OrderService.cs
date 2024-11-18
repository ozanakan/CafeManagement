using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Order;
using CafeOrderManager.Model.Dto.Product;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Service.OrderItem;
using CafeOrderManager.Storage.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace CafeOrderManager.Service.Order
{

    public class OrderService : BaseService<OrderRepository, OrderMapper, OrderDbo, OrderDto, OrderListDto, OrderFilterDto>
    {
        private readonly CategoryRepository _categoryRepository;
        private OrderItemRepository _orderItemRepository;
        private OrderItemMapper _orderItemMapper;
        private ProductRepository _productRepository;


        public OrderService(IAuthService authService, OrderRepository repository, OrderMapper mapper, OrderItemMapper orderItemMapper, OrderItemRepository orderItemRepository, CategoryRepository categoryRepository, ProductRepository productRepository) : base(repository, mapper, authService)
        {
            _categoryRepository = categoryRepository;
            _orderItemRepository = orderItemRepository;
            _orderItemMapper = orderItemMapper;
            _productRepository = productRepository;
        }

        public async override Task<(IEnumerable<OrderListDto> Data, PaginationDto Pagination)> _List(OrderFilterDto filterDto)
        {
            // Verileri getir
            var list = await _repository.List(filterDto);

            // DTO'ya dönüştür
            var mappedList = _mapper.ToListDto(list.Data);

            // Category verilerini getir ve dictionary oluştur
            var categoryList = await _categoryRepository.List(new Model.Dto.Category.CategoryFilterDto { });
            var categoryDict = categoryList.Data.ToDictionary(c => c.Id, c => c.CategoryName);

            // Sonuçları tutmak için bir liste oluştur
            var updatedOrderList = new List<OrderListDto>();

            // Category bilgilerini doldur ve array'e pushla
            foreach (var orderDto in mappedList)
            {
                foreach (var itemDto in orderDto.OrderItemList)
                {
                    itemDto.CategoryName = categoryDict.TryGetValue(itemDto.CategoryId, out var categoryName)
                        ? categoryName
                        : "Unknown";
                }

                // Güncellenmiş OrderDto'yu listeye ekle
                updatedOrderList.Add(orderDto);
            }

            // Güncellenmiş listeyi döndür
            return (updatedOrderList, list.Pagination);
        }

        public override async Task<IEnumerable<OrderDbo>> _Create(OrderDto dto)
        {
            var result = new Result<OrderDbo>();
            try
            {

                var orderDbo = await _repository.Create(_mapper.ToCreate(dto));
                // Order tablosuna verimizi ekliyoruz.

                // OrderItemList içindeki tüm verileri ekliyoruz. Eklerken order tablosuna kayıt olan Id'yi atıyoruz.
                foreach (var orderItemDto in dto.OrderItemList)
                {
                    orderItemDto.OrderId = orderDbo.Id;
                    await _orderItemRepository.Create(_orderItemMapper.ToCreate(orderItemDto));
                }

                //Ürünlerin listesini çekiyoz.
                var productList = await _productRepository.List(new ProductFilterDto { Track = true });

                foreach (var orderItem in dto.OrderItemList)
                {
                    // İlgili ürünleri bul
                    var product = productList.Data.FirstOrDefault(x => x.Id == orderItem.ProductId);
                    if (product == null)
                        continue; // Ürün bulunamazsa döngüye devam et

                    //Stok kontrolü yap
                    if (product.StockQuantity < orderItem.Quantity)
                    {
                        throw new CustomException("validation.insufficient.product.stock");
                    }

                    // Stok miktarını düşür
                    product.StockQuantity -= orderItem.Quantity;
                }

                // Tüm ürünleri toplu olarak güncelle
                await _productRepository.Update(productList.Data); // BatchUpdate metodu varsayılıyor

                return new List<OrderDbo> { orderDbo };

            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return new List<OrderDbo>();
        }

        public async override Task<bool> _Update(OrderDbo dbo, OrderDto dto)
        {

            var result = new Result<OrderDbo>();
            try
            {
                // Order tablosundaki veriyi güncelliyoruz.
                var existingOrder = await _repository.Detail((int)dto.Id);
                if (existingOrder == null)
                {
                    throw new CustomException("validation.order.not.found");
                }

                // Güncelleme işlemi
                var updatedOrder = await _repository.Update(_mapper.ToUpdate(existingOrder, dto));

                // Mevcut OrderItem'ları al
                var existingOrderItems = await _orderItemRepository.List(new Model.Dto.OrderItem.OrderItemFilterDto { OrderId = (int)dto.Id });

                // Yeni ve mevcut OrderItem'ları ayır
                var newOrderItems = dto.OrderItemList.Where(x => !existingOrderItems.Data.Any(y => y.Id == x.Id)).ToList();
                var updatedOrderItems = dto.OrderItemList.Where(x => existingOrderItems.Data.Any(y => y.Id == x.Id)).ToList();

                // Mevcut ürünleri güncelle
                foreach (var updatedItem in updatedOrderItems)
                {
                    var orderItem = existingOrderItems.Data.FirstOrDefault(x => x.Id == updatedItem.Id);
                    if (orderItem != null)
                    {
                        // Stok kontrolü yap
                        var product = await _productRepository.Detail(updatedItem.ProductId);
                        if (product == null)
                            throw new CustomException("validation.product.not.found");

                        // Stok güncelleme (eski miktarı geri ekleyip yeni miktarı çıkarıyoruz)
                        product.StockQuantity += orderItem.Quantity; // Eski stok miktarını geri ekle
                        if (product.StockQuantity < updatedItem.Quantity)
                            throw new CustomException("validation.insufficient.product.stock");

                        product.StockQuantity -= updatedItem.Quantity;

                        await _orderItemRepository.Update(_orderItemMapper.ToUpdate(orderItem, updatedItem));
                    }
                }

                // Yeni ürünleri ekle
                foreach (var newItem in newOrderItems)
                {
                    newItem.OrderId = (int)dto.Id;

                    // Stok kontrolü yap
                    var product = await _productRepository.Detail(newItem.ProductId);
                    if (product == null)
                        throw new CustomException("validation.product.not.found");

                    if (product.StockQuantity < newItem.Quantity)
                        throw new CustomException("validation.insufficient.product.stock");

                    product.StockQuantity -= newItem.Quantity;

                    await _orderItemRepository.Create(_orderItemMapper.ToCreate(newItem));
                }

                // Silinmesi gereken OrderItem'ları belirle
                var deletedOrderItems = existingOrderItems.Data
                    .Where(x => !dto.OrderItemList.Any(y => y.Id == x.Id))
                    .ToList();


                // Güncellenmiş stok bilgilerini toplu olarak kaydet
                var productList = await _productRepository.List(new ProductFilterDto { Track = true });
                await _productRepository.Update(productList.Data);

                return true;
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return true;
        }


        public async Task<Result<bool>> OrderStatusUpdate(OrderDto model)
        {
            var result = new Result<bool>();

            try
            {
                // 1. Sipariş detaylarını getir
                var dbo = await _repository.Detail((int)model.Id);
                if (dbo == null)
                {
                    // Sipariş bulunamazsa hata dön
                    result.Error(new Exception("Sipariş bulunamadı"));
                    return result;
                }

                // Sipariş durumunu güncelle
                dbo.OrderStatus = model.OrderStatus;
                var updateResult = await _repository.Update(dbo);


                // İşlem başarılı
                result.Success(true);


            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }


    }
}