import { combineReducers } from 'redux'
import { connectRouter } from 'connected-react-router'
import * as auth from "./auth/reducers"
import * as apiCall from "./apiCall/reducers"
// import * as modal from "./modal/reducers"

export default history =>
  combineReducers({
    router: connectRouter(history),
    auth: auth.reducer,
    apiCall: apiCall.reducer,
    // modal: modal.reducer
  })
