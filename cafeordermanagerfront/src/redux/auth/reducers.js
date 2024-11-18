
import { createReducer, createActions } from "reduxsauce";
import produce from "immer";


const INITIAL_STATE = {
    token: "",
    userInfo: {},
    roleAccess: [],
    isUserAuthorized: false
};

const { Types, Creators } = createActions({
    setAuthState: ["payload"],
    login: ["payload"],
    loadUserInfo: ["payload"],
});

export const ActionTypes = Types;
export const Actions = Creators;

export const reducer = createReducer(INITIAL_STATE, {
    [Types.SET_AUTH_STATE]: produce((draft, { payload }) => ({ ...draft, ...payload })),
});
