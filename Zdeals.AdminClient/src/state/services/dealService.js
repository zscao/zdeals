import { get, post } from './api';

import { apiRoutes } from './apiRoutes';

export const searchDeals = params => {
  return get(apiRoutes.deals.search, params, 'Failed to search deals.');
}

export const createDeal = request => {
  return post(apiRoutes.deals.create, request, 'Failed to create deal.', 'Deal is created.');
}