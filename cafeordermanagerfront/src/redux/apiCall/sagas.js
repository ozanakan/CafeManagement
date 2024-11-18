
import { call, put, select, takeEvery } from "redux-saga/effects";
import apiService from "../../services/api/apiService";
import { ActionTypes, Actions } from "./reducers";
import { message } from "antd";

function* BaseApiCall({ payload }) {

    if (!navigator.onLine)  //checking the internet
        return;

    const token = yield select(state => state.auth.token); // get bearer token
    const apiCall = yield call(apiService.makeApiCall, { // make Request
        url: payload?.url,
        query: payload?.query,
        data: payload?.data,
        method: payload?.method,
        serviceType: payload.serviceType,
        token: payload.token,
        saveFilter: payload?.saveFilter,
        objectKeys: payload?.objectKeys,
    });
    // #region run callbacks
    if (apiCall?.success && payload?.onSuccess instanceof Function) // run onSuccess
    {
        yield call(payload.onSuccess, { data: apiCall?.data, pagination: apiCall?.pagination });
    }

    if (apiCall?.error && payload?.onError instanceof Function) // run onErorr
        yield call(payload.onError, { data: apiCall?.data, errorMessage: apiCall?.errorMessage, errorMessageTechnical: apiCall?.errorMessageTechnical });

    if (payload?.callback instanceof Function) // run callback
        yield call(payload.callback, apiCall) //
    // #endregion
    if (apiCall?.error && payload?.showAlertOnError) // show alert on Error
        message.error(apiCall?.errorMessage, apiCall?.useI18next);

}
function* Get({ payload }) {
    yield put(Actions.BaseApiCall({
        ...payload,
        method: "get"
    }))
}
function* Post({ payload }) {
    yield put(Actions.BaseApiCall({
        ...payload,
        method: "post"
    }))
}
function* Put({ payload }) {
    yield put(Actions.BaseApiCall({
        ...payload,
        itemId: payload?.itemId,
        method: "put",
    }))
}
function* Delete({ payload }) {
    yield put(Actions.BaseApiCall({
        ...payload,
        method: "delete",
    }))
}

export default [
    takeEvery(ActionTypes.DELETE, Delete),
    takeEvery(ActionTypes.POST, Post),
    takeEvery(ActionTypes.PUT, Put),
    takeEvery(ActionTypes.GET, Get),
    takeEvery(ActionTypes.BASE_API_CALL, BaseApiCall),
];
