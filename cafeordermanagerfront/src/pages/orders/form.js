import React, { useCallback, useState } from "react";
import { useDispatch } from 'react-redux';
import { Actions as ApiCallActions } from '../../redux/apiCall/reducers';
import { OrderStatusEnum, ServiceTypeEnum, StatusEnum } from "../../utils/enums";
import '../../assets/css/order.scss'
import FormSelect from "../../components/form/form-select";
import i18next from "i18next";
import useRefState from "../../utils/use-ref-state";
import FormInput from "../../components/form/form-input";
import { DeleteOutlined } from '@ant-design/icons';
import Validator from '../../components/validator'
import { useNavigate } from "react-router-dom";
import { message } from "antd";

const OrderForm = ({ }) => {
    const navigate = useNavigate();
    const [validator, validatorRef,] = useRefState(new Validator());
    const validatorScopeKey = validator.generateNewScopeKey()
    const dispatch = useDispatch();
    const [formProps, formPropsRef, setFormProps] = useRefState({
        orderStatus: OrderStatusEnum.Pending,
        status: StatusEnum.Active,
        createdDate: new Date(),
        orderItemList: []
    })
    const updateFormProps = values => setFormProps(curr => ({ ...curr, ...values }))

    const onChangeTable = useCallback((table) => {
        updateFormProps({ tableId: table?.value, tableName: table?.name })
    }, [])
    const onChangeOrderNumber = useCallback((val) => { updateFormProps({ orderNumber: val }) }, [])
    const addOrderItem = useCallback(() => {
        let orderItemList = formPropsRef.current.orderItemList || [];
        orderItemList.push({
            quantity: 0
        })
        updateFormProps({ orderItemList })
    }, [])

    const deleteOrderItem = useCallback((index) => {
        let orderItemList = formPropsRef.current.orderItemList || [];
        orderItemList.splice(index, 1);
        updateFormProps({ orderItemList })
    }, [])

    const onChangeCategory = (orderIndex) => (category) => {
        let orderItemList = formPropsRef.current.orderItemList || [];
        orderItemList[orderIndex].categoryId = category?.value;
        orderItemList[orderIndex].categoryName = category?.name;
        updateFormProps({ orderItemList })
    }

    const onChangeProduct = (orderIndex) => (product) => {
        let orderItemList = formPropsRef.current.orderItemList || [];
        orderItemList[orderIndex].productId = product?.value;
        orderItemList[orderIndex].productName = product?.name;
        orderItemList[orderIndex].quantity = 1;
        orderItemList[orderIndex].quantityForValidation = product?.data?.stockQuantity;
        updateFormProps({ orderItemList })
    }

    const onChangeQuantity = (orderIndex) => (val) => {
        let orderItemList = formPropsRef.current.orderItemList || [];
        orderItemList[orderIndex].quantity = val;
        updateFormProps({ orderItemList })
    }


    const save = useCallback(() => {
        let isValid = validator.allValid();
        if (isValid) {
            dispatch(
                ApiCallActions.Post({
                    url: "order",
                    serviceType: ServiceTypeEnum.User,
                    data: formPropsRef.current,
                    showAlertOnError: true,
                    showLoading: true,
                    onSuccess: async ({ data }) => {
                        message.success("Sipariş başarıyla eklendi")
                        navigate('/orders')
                    },
                    // callback: () => setLoading(false),
                })
            );
        }
    }, [formProps])

    return (
        <div style={{}} className="mt-3">
            <div div style={{ borderRadius: 10, paddingTop: 1, background: '#fff' }}>
                <div style={{ borderBottom: '1px solid #ccc', width: '100%', textAlign: 'center' }}><h2>Sipariş Ekleme Formu</h2></div>
                <div className="col-12 row">
                    <div className="col-12 col-sm-12 col-md-6 col-lg-4 mt-3">
                        <FormSelect
                            url="table/dropdown"
                            serviceType={ServiceTypeEnum.User}
                            label={i18next.t("Masa")}
                            selectedId={formProps?.tableId}
                            selectedName={formProps?.tableName}
                            isUiFilter
                            onChange={onChangeTable}
                        />

                    </div>
                    <div className="col-12 col-sm-12 col-md-6 col-lg-4 mt-3">
                        <FormInput
                            label={i18next.t("Sipariş Numarası")}
                            onChange={onChangeOrderNumber}
                            value={formProps?.orderNumber}
                            errorMessage={validator.register("orderNumber", formProps?.orderNumber, [{ rule: "required" }], validatorScopeKey)}
                        />
                    </div>
                </div>
                <div className="col-12">
                    <div onClick={() => addOrderItem()} className="add-item-button">Ürün Ekle</div></div>

                <div style={{ padding: 10 }} >
                    {formProps?.orderItemList?.map((orderItem, orderIndex) => {
                        return (
                            <div style={{ position: 'relative', borderRadius: 10, border: '1px solid #ccc', display: 'flex', flexDirection: 'row', flexWrap: 'wrap', paddingRight: 10 }} className=" mt-3" key={orderIndex}>
                                <div onClick={() => deleteOrderItem(orderIndex)} style={{ position: 'absolute', right: 8, top: 5 }}><DeleteOutlined style={{ fontSize: 18, color: 'red' }} /></div>
                                <div className="col-12 col-sm-12 col-md-6 col-lg-4 mt-3">
                                    <FormSelect
                                        url="category/dropdown"
                                        serviceType={ServiceTypeEnum.User}
                                        label={i18next.t("Kategori")}
                                        selectedId={orderItem?.categoryId}
                                        selectedName={orderItem?.categoryName}
                                        isUiFilter
                                        onChange={onChangeCategory(orderIndex)}
                                    />
                                </div>
                                {orderItem?.categoryId &&
                                    <div className="col-12 col-sm-12 col-md-6 col-lg-4 mt-3">
                                        <FormSelect
                                            query={{
                                                categoryId: orderItem?.categoryId
                                            }}
                                            url="product/dropdown"
                                            serviceType={ServiceTypeEnum.User}
                                            label={i18next.t("Ürün")}
                                            selectedId={orderItem?.productId}
                                            selectedName={orderItem?.productName}
                                            isUiFilter
                                            onChange={onChangeProduct(orderIndex)}
                                        />
                                    </div>}
                                {orderItem?.productId &&
                                    <div className="col-12 col-sm-12 col-md-6 col-lg-3 mt-3">
                                        <FormInput
                                            helperMessage={"Kalan Adet: " + orderItem?.quantityForValidation}
                                            label={i18next.t("Adet") + " *"}
                                            onChange={onChangeQuantity(orderIndex)}
                                            value={orderItem?.quantity}
                                            errorMessage={validator.register('quantity', orderItem?.quantity, [{ rule: 'required' }, { rule: 'isNumber' }, { rule: "maxNumber", value: orderItem?.quantityForValidation }], validatorScopeKey,)}
                                        />
                                    </div>}
                            </div>
                        );
                    }
                    )}
                </div>

                <div className="col-12" style={{ padding: '10px 20px 10px 0px', justifyContent: 'end', display: 'flex' }}>
                    <div className="order-save-button" onClick={() => save()}>Kaydet</div>
                </div>
            </div>
        </div>
    );
};
export default OrderForm;
