import * as types from './types';

const defaultState = {
  loadings: []
}

export default function api(state = defaultState, action) {
  switch(action.type) {
    case types.API_START:

      if(typeof(action.payload) === 'string') {
        return {
          ...state,
          loadings: [...state.loadings, action.payload]
        }
      }
      else {
        return state;
      }
    
    case types.API_END:
      if(typeof(action.payload) === 'string') {
        const loadings = [...state.loadings];
        const index = loadings.indexOf(action.payload);
        if(index >= 0) loadings.splice(index, 1);
        return {
          ...state,
          loadings
        }
      }
      else {
        return state;
      }

    default: return state;
  }
}