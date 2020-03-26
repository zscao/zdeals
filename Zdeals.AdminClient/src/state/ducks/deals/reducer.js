import * as types from './types';

const defaultState = {
  search: {
    query: null,
    result: null,
    error: null,
    loading: false,
    invalid: false,
  }
}

function deals(state = defaultState, action) {
  switch (action.type) {

    case types.SEARCH_DEALS:
      return {
        ...state, search: {
          ...state.search,
          result: action.payload,
          loading: false,
          invalid: false,
        }
      };

    default:
      return state;
  }
}


export default deals;