import * as types from './types';
import apiFetch from '../api/apiFetch';
import apiRoutes from '../api/apiRoutes';


export const searchStores = data => {
  return apiFetch({
    url: apiRoutes.stores.base,
    label: types.SEARCH_STORES,
    data,
    //onSuccess: data => ({type: types.SEARCH_STORES, payload: data}),
  })
}

export const createStore = data => {
  return apiFetch({
    url: apiRoutes.stores.base,
    method: 'POST',
    label: types.CREATE_STORE,
    data,
    toast: {
      success: 'Store is created.'
    }
  })
}