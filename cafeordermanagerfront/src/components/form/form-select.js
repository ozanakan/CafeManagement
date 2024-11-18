import { Form } from "antd";
import i18next from "i18next";
import React, { useState, useRef } from "react";
import { useDispatch } from "react-redux";
import { AsyncPaginate } from "react-select-async-paginate";
import { Actions as ApiCallActions } from "../../redux/apiCall/reducers";
import general from "../../utils/general";
import FloatLabel from "../float-label";
import useRefState from "../../utils/use-ref-state";
import { ServiceTypeEnum } from "../../utils/enums";

const FormSelect = ({
  label,
  selectedId,
  selectedName,
  errorMessage,
  antdFormItemProps = {},
  onChange,
  url,
  action = "dropdown",
  disabled,
  query,
  searchTextKey = "searchText",
  isClearable = true,
  rightAction,
  getOptionLabel = (option) => option?.name,
  nameKey = "name",
  valueKey = "value",
  noRecordText = "general.no_records_found",
  show = true,
  isTranslate = true,
  className = "",
  isUiFilter = false,
  serviceType,
  isMulti = false,
  value,
  notDeletedList = [], //Silinmesini istemediğimiz value listesi.
  notDeletedMessage = "general.record_not_deleted_message",
  customSearch = false,
  clearIndicator = false, //Silme butonunu kaldırmak için kullanılır.
  customSearchText = "general.enter_three_letters", //customsearch için menu içinde yazması gereken mesaj.
  sortName = true,
  closeMenuOnSelect = true, //Seçim yapıldığında menüyü kapatmak için kullanılır.
  helperMessage = "",
  size = "large",
}) => {
  const dispatch = useDispatch();
  const [clearCache, setClearCache] = useState([
    general.generateRandomString(10),
  ]);
  const [searchTextValue, searchTextValueRef, setSearchTextValue] =
    useRefState(null);
  const timeoutRef = useRef(null);

  const onCahngeEvent = (e, option) => {
    //Select içerisinde silinmek istemeyen elemanlar varsa kontrol edilir.Silinmesi engellenir.
    if (notDeletedList.length > 0) {
      {
        const missingValues = notDeletedList.filter(
          (item) => !e.some((eItem) => eItem.value === item)
        );
        if (missingValues.length > 0) {
          general.messageError(i18next.t(notDeletedMessage));
          return;
        }
      }
    }
    if (onChange instanceof Function)
      onChange(e ? e : { value: null, name: null, key: null, data: null }, option);
  };

  const loadOptions = (search, loadedOptions, { page }) => {
    const handleSearch = (searchText) => {
      return new Promise((resolve, reject) => {
        var searchTextObj = {};
        searchTextObj[searchTextKey] = searchText;

        let orderByObj = {};
        if (sortName) {
          orderByObj = {
            order_by: "name",
            order_type: "asc",
          };
        }

        if (show) {
          dispatch(
            ApiCallActions.Post({
              url: url,
              serviceType: ServiceTypeEnum.User,
              showAlertOnError: true,
              data: {
                ...query
              },

              onSuccess: ({ data, pagination }) => {

                const hasMore = general.isNullOrEmpty(pagination)
                  ? false
                  : pagination?.pageCount > pagination?.pageNumber;
                let translateData = data.map((e) => {
                  e.key = e[nameKey];
                  e.name = isTranslate ? i18next.t(e[nameKey]) : e[nameKey];
                  e.value = e[valueKey];
                  return e;
                });

                resolve({
                  options: translateData,
                  hasMore: hasMore,
                  additional: {
                    page: page + 1,
                  },
                });
              },
              onError: ({ errorMessage }) => {
                // reject(i18next.t(errorMessage))
              },
            })
          );
        } else {
          resolve({
            options: [],
          });
        }
      });
    };

    if (customSearch && search.length < 3) {
      return Promise.resolve({ options: [] });
    }

    if (search?.length >= 3 && customSearch) {
      clearTimeout(timeoutRef.current);
      return new Promise((resolve) => {
        timeoutRef.current = setTimeout(() => {
          handleSearch(search).then(resolve);
        }, 650);
      });
    } else {
      return handleSearch(search);
    }
  };
  const handleInputChange = (inputValue, { action }) => {
    setSearchTextValue(inputValue);
  };

  const noOptMsg = () =>
    customSearch &&
      (general.isNullOrEmpty(searchTextValue) || searchTextValue?.length < 3)
      ? i18next.t(customSearchText)
      : i18next.t(noRecordText);
  const loadingMsg = () => i18next.t("general.loading");

  const getStyles = {
    control: (baseControl) => ({
      ...baseControl,
      borderRadius: 6,
      borderColor: general.isNullOrEmpty(errorMessage) ? "#CECECE" : "#f5222e",
      minHeight: "25px",
      "&:hover": {
        borderColor: "#1677ff",
      },
    }),
    container: (baseContainer) => ({
      ...baseContainer,
      width: "100%",
    }),
    menuList: (base) => ({
      ...base,
      maxHeight: 150,
    }),
    menuPortal: (base) => ({ ...base, zIndex: 9999 }),
    input: (baseInput) => ({
      ...baseInput,
      padding: 0,
      margin: 0,
    }),

  };
  let val = {};
  val.name = selectedName;
  val.value = selectedId;
  return (
    <Form.Item
      /*  label={label} */
      validateStatus={general.isNullOrEmpty(errorMessage) ? "" : "error"}
      help={errorMessage ? errorMessage : helperMessage}
      {...antdFormItemProps}
    >
      <FloatLabel size={size} label={label} name={label}
        value={Array.isArray(value) && value.length > 0 ? value : (!general.isNullOrEmpty(selectedId) ? val : null)}
        disabled={disabled}
      >
        <AsyncPaginate
          cacheUniqs={clearCache}
          isClearable={isClearable}
          getOptionLabel={getOptionLabel}
          onMenuClose={() => setClearCache([general.generateRandomString(10)])}
          debounceTimeout={100}
          placeholder={""}
          className={"border-style " + className}
          menuPosition="fixed"
          menuPlacement="auto"
          isDisabled={disabled}
          styles={getStyles}
          noOptionsMessage={noOptMsg}
          loadingMessage={loadingMsg}
          value={!general.isNullOrEmpty(value) ? value : (!general.isNullOrEmpty(selectedId) ? val : null)}
          loadOptions={loadOptions}
          onChange={onCahngeEvent}
          components={
            clearIndicator ? { ClearIndicator: () => null } : undefined
          }
          additional={{
            page: 1,
          }}
          isMulti={isMulti}
          closeMenuOnSelect={isMulti === true ? false : closeMenuOnSelect}
          onInputChange={handleInputChange}
        />
        {rightAction}
      </FloatLabel>
    </Form.Item>
  );
};

const areEqual = (prev, next) => {
  return (
    prev.label === next.label &&
    prev.placeholder === next.placeholder &&
    prev.noRecordText === next.noRecordText &&
    prev.selectedId === next.selectedId &&
    prev.searchTextKey === next.searchTextKey &&
    prev.disabled === next.disabled &&
    JSON.stringify(prev.query) === JSON.stringify(next.query) &&
    prev.url === next.url &&
    prev.action === next.action &&
    prev.selectedName === next.selectedName &&
    prev.errorMessage === next.errorMessage &&
    prev.value === next.value &&
    prev.isMulti === next.isMulti &&
    prev.show === next.show &&
    prev.notDeletedList === next.notDeletedList &&
    prev.filterOptionsByValue === next.filterOptionsByValue &&
    prev.filterOptionsByMultiValue === next.filterOptionsByMultiValue &&
    prev.closeMenuOnSelect === next.closeMenuOnSelect
  );
};

export default React.memo(FormSelect, areEqual);
