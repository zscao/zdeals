import * as actions from './actions';
import * as services from '../../services/dealService';

export const searchDeals = request => dispatch => {
  dispatch(actions.searchDealRequest(request));

  return services.searchDeals(request)
    .then(response => {
      dispatch(actions.searchDealSuccess(response))
      return response;
    })
    .catch(error => {
      dispatch(actions.searchDealFailure(error))
      return error;
    })
}
