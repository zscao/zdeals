import axios from 'axios';
import { toast } from 'react-toastify';


export function post(url, data, failureToast, successToast) {
  return axios.post(url, data)
    .then(response => {
      if(successToast) toast.success(successToast);
      return response.data;
    })
    .catch(handleAxiosError(failureToast));
}


export function get(url, params, failureToast, successToast) {
  return axios.get(url, params)
    .then(response => {
      if(successToast) toast.success(successToast);
      return response.data;
    })
    .catch(handleAxiosError(failureToast));
}

export function put(url, params, failureToast, successToast) {
  return axios.put(url, params)
    .then(response => {
      if(successToast) toast.success(successToast);
      return response.data;
    })
    .catch(handleAxiosError(failureToast));
}

export function handleAxiosError(message) {
  return function(e) {
    let error = e;
      if(e.isAxiosError) {
        e = e.toJSON();
        // console.log('Axios error: ', e);
        error = { code: e.code || 400, message: e.message}
      }
      // console.log(error);
      const failure = message || 'An error occurred.';
      toast.error(failure + ' Message: ' + error.message || error, {autoClose: false});
      
      return Promise.reject(error);
  }
}

