import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch } from 'react-redux';
import { Actions as ApiCallActions } from '../../redux/apiCall/reducers';
import { OrderStatusEnum, ServiceTypeEnum } from "../../utils/enums";
import Picture1 from '../../assets/img/foot/picture1.jpg';
import FalseIcon from '../../assets/img/vector/false.png';
import TrueIcon from '../../assets/img/vector/true.png';
import '../../assets/css/order.scss'
import { useNavigate } from "react-router-dom";
import { Tooltip } from "antd";
import { get } from "store";
import i18next from "i18next";

const Orders = ({ }) => {
    const navigate = useNavigate();
    const dispatch = useDispatch();
    const [orderList, setOrderList] = useState([]);
    useEffect(() => {
        getOrderList()
    }, [])
    const newOrder = () => {
        navigate('/order/form')
    }
    const getOrderList = () => {
        dispatch(ApiCallActions.Post({
            url: "order/list",
            serviceType: ServiceTypeEnum.User,
            showAlertOnError: true,
            onSuccess: async ({ data }) => {
                setOrderList(data);
            }
        }))
    }
    const updateOrderStatus = (orderId, status) => {
        dispatch(ApiCallActions.Post({
            url: "order/orderStatusUpdate",
            serviceType: ServiceTypeEnum.User,
            data: {
                id: orderId,
                orderStatus: status
            },
            showAlertOnError: true,
            onSuccess: async ({ data }) => {
                if (data === true)
                    getOrderList()
            }
        }))
    }

    return (
        <>
            <div className="new-order-button" onClick={() => newOrder()}>Yeni Sipariş Oluştur</div>
            <div className="col-12 row order-list">
                {orderList.map((item, index) => {
                    return (
                        <div className="col-12 col-sm-12 col-md-6 col-lg-4 mt-2 order-card" key={index}>
                            <div className="card-content">
                                <div className="card-header">
                                    <span className="order-number">Sipariş Numarası: {item.orderNumber} </span>-
                                    <span className="table-id"> Masa Id: {item.tableId}</span>
                                </div>
                                <div className="order-items">
                                    {item.orderItemList.map((orderItem, orderIndex) => {
                                        return (
                                            <div className="order-item" key={orderIndex}>
                                                <div className="item-image">
                                                    <img src={Picture1} alt="Product" />
                                                </div>
                                                <div className="item-details">
                                                    <div className="product-name">Ürün Adı: {orderItem.productName}</div>
                                                    <div className="category-name">Kategori: {orderItem.categoryName}</div>
                                                    <div className="price-quantity">
                                                        <div className="price">{orderItem.productPrice}₺</div>
                                                        <div className="quantity">Adt: {orderItem.quantity}</div>
                                                    </div>
                                                </div>
                                            </div>
                                        );
                                    })}
                                </div>
                                <div className="card-footer">
                                    {item.orderStatus === OrderStatusEnum.Pending || item.orderStatus === OrderStatusEnum.InProgress ?
                                        (<>
                                            {item.orderStatus === OrderStatusEnum.InProgress && <div className="pending-status-text">{i18next.t(OrderStatusEnum.Name[item.orderStatus])}</div>}
                                            <Tooltip title="İptal Et">
                                                <div onClick={() => updateOrderStatus(item.id, OrderStatusEnum.Cancelled)} className="icon-container false-icon">
                                                    <img src={FalseIcon} alt="False Icon" />
                                                </div>  </Tooltip>
                                            {item.orderStatus === OrderStatusEnum.Pending &&
                                                <Tooltip title="Hazırlanıyor">
                                                    <div onClick={() => updateOrderStatus(item.id, OrderStatusEnum.InProgress)} className="icon-container true-icon">
                                                        <img src={TrueIcon} alt="True Icon" />
                                                    </div>
                                                </Tooltip>}
                                            {item.orderStatus === OrderStatusEnum.InProgress &&
                                                <Tooltip title="Tamamlandı">
                                                    <div onClick={() => updateOrderStatus(item.id, OrderStatusEnum.Completed)} className="icon-container true-icon">
                                                        <img src={TrueIcon} alt="True Icon" />
                                                    </div>
                                                </Tooltip>}
                                        </>) :
                                        (
                                            <div className={`status-text${item.orderStatus === OrderStatusEnum.Cancelled ? "-cancelled" : ""}`}>{i18next.t(OrderStatusEnum.Name[item.orderStatus])}</div>
                                        )
                                    }
                                </div>
                            </div>
                        </div>
                    );
                })}
            </div>
        </>
    );
};
export default Orders;
