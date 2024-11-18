
import { createReducer, createActions } from "reduxsauce";

const INITIAL_STATE = {
};

const { Types, Creators } = createActions({
    BaseApiCall: ["payload"],
    Post: ["payload"],
    Delete: ["payload"],
    Put: ["payload"],
    Get: ["payload"],
});

export const ActionTypes = Types;
export const Actions = Creators;

export const reducer = createReducer(INITIAL_STATE, {

});
