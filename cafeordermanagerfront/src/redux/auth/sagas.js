import { put, takeEvery } from "redux-saga/effects";
import { ActionTypes, Actions } from "./reducers";


function* login({ payload }) {
    yield put(Actions.setAuthState({ t: 0 }));
}

function* loadUserInfo({ payload }) {
    yield put(Actions.setAuthState({ t: 1 }));
}


export default [
    takeEvery(ActionTypes.LOGIN, login),
    takeEvery(ActionTypes.LOAD_USER_INFO, loadUserInfo),
];
