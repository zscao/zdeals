import * as types from './types';

export const apiStart = label => ({
  type: types.API_START,
  payload: label
})

export const apiEnd = label => ({
  type: types.API_END,
  payload: label
})

export const apiSuccess = (label, data, request) => ({
  type: types.API_SUCCESS,
  payload: {
    label,
    data,
    request
  }
})

export const apiError = error => ({
  type: types.API_ERROR,
  error
})