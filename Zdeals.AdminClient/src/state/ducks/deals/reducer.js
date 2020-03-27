import * as types from './types';
import * as apiTypes from '../api/types';

const defaultState = {
  search: {
    query: null,
    result: null,
  }
}

function deals(state = defaultState, action) {
  switch (action.type) {

    case apiTypes.API_SUCCESS:
      return handleApiSuccessAction(state, action.payload);

    default:
      return state;
  }
}

function handleApiSuccessAction(state, payload) {
  if (!payload.label) return state;

  switch (payload.label) {

    case types.SEARCH_DEALS:
      return {
        ...state, search: {
          query: payload.query,
          result: payload.data,
        }
      };

    default:
      return state;
  }
}


export default deals;