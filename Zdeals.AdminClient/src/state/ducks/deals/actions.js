import * as types from './types';
import apiFetch from '../api/apiFetch';
import apiRoutes from '../api/apiRoutes';

export const searchDeals = data => {
  return apiFetch({
    url: apiRoutes.deals.base,
    label: types.SEARCH_DEALS,
    data,
    //onSuccess: data => ({ type: types.SEARCH_DEALS, payload: data }),
  });
}

export const getDealById = id => {
  return apiFetch({
    url: `${apiRoutes.deals.base}/${id}`,
    label: types.GET_DEAL_BY_ID,
    //onSuccess: data => { console.log('data: ', data)}
    //onFailure: error => { console.log('error: ', error)}
  });
}

export const createDeal = data => {
  return apiFetch({
    url: apiRoutes.deals.base,
    method: 'POST',
    label: types.CREATE_DEAL,
    data,
    toast: {
      success: 'Deal created.'
    },
  })
}

export const updateDeal = (id, data) => {
  const url = `${apiRoutes.deals.base}/${id}`;
  return apiFetch({
    url,
    method: 'PUT',
    label: types.EDIT_DEAL,
    data,
    toast: {
      success: 'Deal saved.'
    },
  })
} 