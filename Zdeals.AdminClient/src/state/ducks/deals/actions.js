import * as types from './types';

export const searchDealRequest = request => ({
  type: types.SEARCH_DEAL_REQUEST,
  payload: request
})

export const searchDealSuccess = result => ({
  type: types.SEARCH_DEAL_SUCCESS,
  payload: result
})

export const searchDealFailure = error => ({
  type: types.SEARCH_DEAL_FAILURE,
  payload: error
})