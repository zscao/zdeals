import * as types from './types';

export const apiStart = (label, data) => ({
  type: types.API_START,
  payload: label
})

export const apiEnd = (label, data) => ({
  type: types.API_END,
  payload: label
})

export const apiError = error => ({
  type: types.API_ERROR,
  error
})