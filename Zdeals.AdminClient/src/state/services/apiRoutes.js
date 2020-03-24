const baseUrl = 'https://localhost:5001/api/'

export const apiRoutes = {
  deals: {
    create: baseUrl + 'deals',
    search: baseUrl + 'deals',
    getById: baseUrl + 'deals/{0}'
  }
} 