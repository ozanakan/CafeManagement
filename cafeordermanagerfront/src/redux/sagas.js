import { all } from 'redux-saga/effects'
import auth from "./auth/sagas"
import apiCall from "./apiCall/sagas"

export default function* rootSaga() {
  yield all([
    ...auth,
    ...apiCall
  ])
}
