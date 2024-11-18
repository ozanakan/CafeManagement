const { default: i18next } = require("i18next");

const isExisty = function (value) {
  return value !== null && value !== undefined;
};

const isEmpty = function (value) {
  if (value instanceof Array) {
    return value.length === 0;
  }
  return value === "" || !isExisty(value);
};

const isEmptyTrimed = function (value) {
  if (typeof value === "string") {
    return value.trim() === "";
  }
  return true;
};

const validations = {
  matchRegexp: (value, regexp) => {
    const validationRegexp =
      regexp instanceof RegExp ? regexp : new RegExp(regexp);
    return isEmpty(value) || validationRegexp.test(value);
  },

  // isPhone: value => validations.matchRegexp(value, /\d{1}((\(\d{3}\) ?)|(\d{3})\s) ?\d{3} \d{2} \d{2}$/g),
  isPhone: (value) => {
    const phoneNumber = value.replace(/[\(\)\s-]/g, ""); // Parantezleri ve boşlukları kaldır
    if (phoneNumber.length === 10) {
      return true; // 11 haneli veya sadece rakamlardan oluşuyorsa geçerli kabul et
    }
    return validations.matchRegexp(
      value,
      /\d{1}((\(\d{3}\) ?)|(\d{3})\s) ?\d{3} \d{2} \d{2}$/g
    ); // Diğer durumlar için regex kontrolü yap
  },
  isIdentityNo: (value) => validations.matchRegexp(value, /\d{11}/g),
  isPlate: (value) =>
    validations.matchRegexp(
      value?.replace(/\s+/g, "")?.toUpperCase(),
      /^(0[1-9]|[1-7][0-9]|8[01])((\s?[a-zA-Z]\s?)(\d{4,5})|(\s?[a-zA-Z]{2}\s?)(\d{3,4})|(\s?[a-zA-Z]{3}\s?)(\d{2,3}))$/
    ),

  isEmail: (value) => validations.matchRegexp(value, /^\S+@\S+\.\S+$/), // /^[a-zA-Z]+[a-zA-Z0-9_.]+@[a-zA-Z0-9.]+[a-zA-Z]$/), // /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i
  isEmpty: (value) => isEmpty(value),

  required: (value) => {
    if (typeof value === "string" || value instanceof String) {
      // alert(value.replace(/\s/g, '').length);

      return (
        !isEmpty(value) &&
        value != "[]" &&
        value.replace(/\s/g, "").length !== 0
      );
    } else return !isEmpty(value);
  },

  trim: (value) => !isEmptyTrimed(value),

  isNumber: (value) => validations.matchRegexp(value, /^\d+$/),
  isNotNumber: (value) => !validations.matchRegexp(value, /\d/),

  isPrice: (value) => validations.matchRegexp(value, /^-?\d*([.](?=\d{1}))?\d{0,2}$/),

  isFloat: (value) => {
    const isFloatx =
      validations.matchRegexp(value, /^(?:-?[1-9]\d*|-?0)?(?:\,+)?$/i) ||
      validations.matchRegexp(value, /^(?:-?[1-9]\d*|-?0)?(?:\,\d+)?$/i) ||
      validations.matchRegexp(value, /^(?:-?[1-9]\d*|-?0)?(?:\.+)?$/i) ||
      validations.matchRegexp(value, /^(?:-?[1-9]\d*|-?0)?(?:\.\d+)?$/i);
    return isFloatx;
  },

  isTrainingReportItem: (value) => validations.matchRegexp(value, /^[\d.:]+$/),

  isPositive: (value) => {
    if (isExisty(value)) {
      return (
        (validations.isNumber(value) || validations.isFloat(value)) &&
        value >= 0
      );
    }
    return true;
  },
  /*  isPLate: (value) => {
         var regex, v;
         v = value.replace(/\s+/g, '').toUpperCase();
         regex = /^(0[1-9]|[1-7][0-9]|8[01])(([A-Z])(\d{4,5})|([A-Z]{2})(\d{3,4})|([A-Z]{3})(\d{2}))$/;
         return v.match(regex)
     }, */

  maxNumber: (value, max) => isEmpty(value) || parseInt(value, 10) <= parseInt(max, 10),
  minNumber: (value, min) => isEmpty(value) || parseInt(value, 10) >= parseInt(min, 10),
  maxYear: (value, max) => isEmpty(value) || parseInt(value, 10) <= parseInt(max, 10),
  minYear: (value, min) => isEmpty(value) || parseInt(value, 10) >= parseInt(min, 10),
  maxFloat: (value, max) => isEmpty(value) || parseFloat(value) <= parseFloat(max),
  minFloat: (value, min) => isEmpty(value) || parseFloat(value) >= parseFloat(min),
  isString: (value) => typeof value === "string" || value instanceof String,
  minStringLength: (value, length) => (value + "").length >= length,
  maxStringLength: (value, length) => (value + "").length <= length,
  minArrayLength: (value, min) => isEmpty(value) || !(value instanceof Array) || value?.length >= min,
  isUrl: (value) => validations.matchRegexp(value, /^(?:[a-zA-Z/0-9_-]+|\d+)$/),
  isNicknameFormatted: (value) => validations.matchRegexp(value, /^[a-z0-9/][a-z0-9_./]+$/), //^\w+$
  isTc: (value) => validations.matchRegexp(value, /^[1-9]{1}[0-9]{9}[02468]{1}$/),
  isPassword: (value) => validations.matchRegexp(value, /^(?=.*[a-z])(?=.*[A-Z]).{6,}$/),

};

const messages = {
  matchRegexp: (i18next, value, regexp) => i18next.t("none"),
  isPhone: (i18next, value) => i18next.t("validation.phone"),
  isIdentityNo: (i18next, value) => i18next.t("validation.identity_no"),
  isEmail: (i18next, value) => i18next.t("validation.email"),
  isEmpty: (i18next, value) => isEmpty(value),
  required: (i18next, value) => i18next.t("validation.required"),
  trim: (i18next, value) => !isEmptyTrimed(value),
  isNotNumber: (i18next, value) => i18next.t("validation.string"),
  isNumber: (i18next, value) => i18next.t("validation.number"),
  isPrice: (i18next, value) => i18next.t("validation.price"),
  isFloat: (i18next, value) => i18next.t("validation.float"),
  isTrainingReportItem: (i18next, value) => i18next.t("validation.training_report_item"),
  isPositive: (i18next, value) => i18next.t("validation.positive_number"),
  areEqual: (i18next, value, value2) => i18next.t("validation.not_equal"),
  maxNumber: (i18next, value, max) => i18next.t("validation.max_number").replace("[number]", max),
  minNumber: (i18next, value, min) => i18next.t("validation.min_number").replace("[number]", min),
  maxFloat: (i18next, value, max) => i18next.t("validation.max_number").replace("[number]", max),
  minFloat: (i18next, value, min) => i18next.t("validation.min_number").replace("[number]", min),
  isString: (i18next, value) => i18next.t("validation.string"),
  minStringLength: (i18next, value, length) => i18next.t("validation.min_string_length").replace("[length]", length),
  maxStringLength: (i18next, value, length) => i18next.t("validation.max_string_length").replace("[length]", length),
  minArrayLength: (i18next, value, min) => i18next.t("validation.min_array_length").replace("[number]", min),
  isUrl: (i18next, value) => i18next.t("validation.isUrl"),
  isPlate: (i18next, value) => i18next.t("validation.invalid_plate"),
  minYear: (i18next, value) => i18next.t("validation.min_year").replace("[number]", value),
  maxYear: (i18next, value) => i18next.t("validation.max_year").replace("[number]", value),
  isNicknameFormatted: (i18next, value) => i18next.t("validation.nickname_format"),
  isTc: (i18next, value) => i18next.t("validation.format.tc"),
  isPassword: (i18next, value) => i18next.t("validation.password"),
};

module.exports = {
  ValidationRules: validations,
  ValidationMessages: messages,
};
