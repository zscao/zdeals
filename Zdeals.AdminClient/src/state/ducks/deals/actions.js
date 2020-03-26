import * as types from './types';
import apiFetch from '../api/apiFetch';
import apiRoutes from '../api/apiRoutes';

export const searchDeals = request => {
  return apiFetch({
    url: apiRoutes.deals.base,
    data: request,
    onSuccess: data => ({ type: types.SEARCH_DEALS, payload: data }),
    label: types.SEARCH_DEALS
  });
}

export const getDealById = id => {
  return apiFetch({
    url: `${apiRoutes.deals.base}/${id}`,
    //onSuccess: data => { console.log('data: ', data)}
    //onFailure: error => { console.log('error: ', error)}
  });
}

export const createDeal = request => {
  return apiFetch({
    method: 'POST',
    url: apiRoutes.deals.base,
    data: request,
  })
}

export const updateDeal = (id, request) => {
  const url = `${apiRoutes.deals.base}/${id}`;
  return apiFetch({
    method: 'PUT',
    url,
    data: request
  })
} 

export const test = () => {
    return {type: 'TEST'}
}