export const baseUrl = 'https://localhost:5001/api'

const apiRoutes = {
  deals: {
    base: baseUrl + '/deals',
  },
  stores: {
    base: baseUrl + '/stores',
  }
} 

export default apiRoutes;
