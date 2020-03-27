import * as types from './types';
import * as apiTypes from '../api/types';

const defaultState = {
  search: {
    query: null,
    result: null,
  }
}

function stores(state = defaultState, action) {
  switch (action.type) {

    case apiTypes.API_SUCCESS:
      return handleApiSuccessAction(state, action.payload);

    default:
      return state;
  }
}

function handleApiSuccessAction(state, payload) {
  if (!payload || !payload.label) return state;

  switch (payload.label) {
    case types.SEARCH_STORES:
      return {
        ...state, search: {
          request: payload.request,
          result: payload.data
        }
      };

    default:
      return state;
  }
}

export default stores;