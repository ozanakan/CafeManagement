import * as UserTypeEnum from './enums/user-type-enum'


const isNullOrEmpty = text => text === undefined || text === '' || text === null || text === NaN

const isNullOrEmptySetValue = value => {
  if (value === undefined || value === '' || value === null || value === NaN) {
    return ''
  }
  return value
}


const generateRandomString = (length = 15) => {
  var result = ''
  var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
  var charactersLength = characters.length
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength))
  }
  return result
}

const generateKeyForUpdateGridItem = id => {
  return { id, key: generateRandomString(10) }
}

const toUpper = text => text?.toLocaleUpperCase('tr-TR')

const removeFormatNumber = number => {
  return number?.toString()?.replace(/\./g, '')
}

const forceLowerCase = value => {
  return value
    .replaceAll('İ', 'i')
    .replaceAll('I', 'ı')
    .toLowerCase()
}

const scrollTopForValidation = (validator) => {
  // This function is scrolling to the wrong position with the help of the classes assigned to the components before, according to the wrong contents coming with the validator.
  var list = [];

  // Ger invalid fields from the validator
  const classNameList = validator?.getInvalidItems();

  // Get the elements from the classNameList
  for (let i = 0; i < classNameList.length; i++) {
    // Get the element from the classNameList
    const items = document.getElementsByClassName(classNameList[i]);

    // Push the elements to the scroll list
    for (let j = 0; j < items.length; j++)
      //  get scroll top of the element and push it to the list
      list.push(items[j]?.getBoundingClientRect()?.top);
  }

  if (list.length > 0) {
    // Get the highest value from the list
    var topItemPosition = list.reduce((a, b) => {
      return Math.min(a, b);
    });

    // Scroll to the highest value
    window.scrollTo(0, window.pageYOffset - 50 + topItemPosition);
  }
};

const wait = ms => {
  return new Promise(resolve => setTimeout(resolve, ms))
}

function convertToUrlFormat(text) {
  // Boşlukları '-' ile değiştir
  let urlSlug = text
    .trim()
    .toLowerCase()
    .replace(/\s+/g, '-')

  // URL'de izin verilmeyen karakterleri çıkar
  urlSlug = urlSlug.replace(/[^a-z0-9\-]/g, '')

  return urlSlug
}

function keysToLowerCase(obj) {
  if (obj instanceof Array) {
    return obj.map(keysToLowerCase)
  } else if (obj !== null && typeof obj === 'object') {
    return Object.keys(obj).reduce((acc, key) => {
      acc[key.charAt(0).toLowerCase() + key.slice(1)] = keysToLowerCase(obj[key])
      return acc
    }, {})
  }
  return obj
}

const isAdmin = () => {
  if (localStorage.getItem('userRole') == UserTypeEnum.Admin) {
    return true
  }
  return false
}


export default {
  isAdmin,
  scrollTopForValidation,
  keysToLowerCase,
  convertToUrlFormat,
  isNullOrEmpty,
  generateRandomString,
  generateKeyForUpdateGridItem,
  wait,
  toUpper,
  removeFormatNumber,
  forceLowerCase,
  isNullOrEmptySetValue,
}
