import axios from 'axios';
import { baseUrl } from './apiRoutes';
import { apiError, apiStart, apiEnd } from './actions';

export default function apiFetch(request) {
  return dispatch => {

    const {
      url = '',
      method = 'GET',
      data = null,
      label = '',
      headers = null,
      onSuccess = null,
      onFailure = null
    } = request;

    if(label)dispatch(apiStart(label));

    const dataOrParams = ['GET', 'DELETE'].includes(method.toUpperCase()) ? 'params' : 'data';
    axios.defaults.baseURL = baseUrl;

    return axios.request({url, method, headers, [dataOrParams]: data})
    .then(response => {
      handleCallback(dispatch, onSuccess, response.data);
      return response.data;
    })
    .catch(e => {
      let error = e;
      if (e.isAxiosError) {
        e = e.toJSON();
        error = { code: e.code || 400, message: e.message }
      }
      handleCallback(dispatch, onFailure, error);
      dispatch(apiError(error));
      return Promise.reject(error);
    })
    .finally(() => {
      if(label)dispatch(apiEnd(label));
    })
  }
}

const handleCallback = (dispatch, callback, data) => {

  if(!callback) return;

  let action = callback;
  if(typeof(callback) === 'function') 
    action = callback(data);

  if(typeof(dispatch) === 'function' && typeof(action) === 'object' && typeof(action.type) === 'string') 
    dispatch(action);
}


