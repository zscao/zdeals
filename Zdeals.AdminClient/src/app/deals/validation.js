export const dealFormValidation = {
  source: {
    required: 'Source is required'
  },
  title: { 
    required: 'Title is required' 
  },
  highlight: { 
    required: 'Highlight is required' 
  },
  dealPrice: { 
    pattern: { 
      value: /^\d*(\.\d{1,2})?$/, 
      message: 'Deal price must be numbers' 
    } 
  },
  fullPrice: { 
    pattern: { 
      value: /^\d*(\.\d{1,2})?$/, 
      message: 'Full price must be numbers' 
    } 
  },
  publishedDate: { 
    required: 'Published date is required' 
  }
}