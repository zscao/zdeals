import { get, post, put } from './api';

import { apiRoutes } from './apiRoutes';

export const searchDeals = params => {
  return get(apiRoutes.deals.base, params, 'Failed to search deals.');
}

export const getDealById = id => {
  const url = `${apiRoutes.deals.base}/${id}`;
  return get(url, undefined, 'Failed to get deal ' + id);
}

export const createDeal = request => {
  return post(apiRoutes.deals.base, request, 'Failed to create deal.', 'Deal is created.');
}

export const updateDeal = (id, request) => {
  const url = `${apiRoutes.deals.base}/${id}`;
  return put(url, request, 'Failed to update deal.', 'Deal is updated.');
}