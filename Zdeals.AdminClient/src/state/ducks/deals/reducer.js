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

    case types.SEARCH_DEAL_REQUEST:
      return {
        ...state, search: {
          ...state.search,
          query: action.payload,
          loading: true,
        }
      };

    case types.SEARCH_DEAL_SUCCESS:
      return {
        ...state, search: {
          ...state.search,
          result: action.payload,
          loading: false,
          invalid: false,
        }
      };

    case types.SEARCH_DEAL_FAILURE:
      return {
        ...state, search: {
          ...state.search,
          error: action.payload,
          loading: false,
          invalid: true
        }
      }

    default:
      return state;
  }
}


export default deals;