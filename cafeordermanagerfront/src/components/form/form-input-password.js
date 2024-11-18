import { KeyOutlined, UndoOutlined } from '@ant-design/icons';
import { Form, Input } from 'antd';
import i18next from 'i18next';
import React, { useCallback, useState } from 'react';
import general from '../../utils/general';
import FloatLabel from '../float-label';

const FormInputPassword = ({
  label,
  placeholder = "form.select.placeholder",
  value,
  errorMessage,
  onChange,
  prefix,
  suffix,
  antdFormItemProps,
  antdInputProps,
  disabled,
  addRequiredSign = false,
  className = "",
  size = "large",
}) => {

  const [generatePasswordClassActive, setGeneratePasswordClassActive] = useState(false);

  const onCahngeEvent = (e) => {
    if (onChange instanceof Function)
      onChange(e.target.value);
  }

  const toggleGeneratePasswordClass = useCallback(() => {
    setGeneratePasswordClassActive(true);
    setTimeout(() => {
      setGeneratePasswordClassActive(false);
    }, 1000);
  }, [generatePasswordClassActive])

  const generatePassword = useCallback(() => {
    if (onChange instanceof Function)
      onChange(general.generateRandomString(10));
    toggleGeneratePasswordClass();
  })

  return (
    <Form.Item
      validateStatus={general.isNullOrEmpty(errorMessage) ? "" : "error"}
      help={errorMessage ?? ""}
      style={{}}
      {...antdFormItemProps}
    >
      <FloatLabel size={size} label={label} name={label} value={value} disabled={false}>
        <Input.Password
          autoComplete='new-password'
          suffix={suffix}
          allowClear
          className={'border-style ' + className}
          value={value}
          disabled={disabled}
          size={size}
          onChange={onCahngeEvent}
          {...antdInputProps}
        />
      </FloatLabel>
    </Form.Item>
  )
}

const areEqual = (prev, next) => {
  return prev.label === next.label
    && prev.placeholder === next.placeholder
    && prev.errorMessage === next.errorMessage
    && prev.disabled === next.disabled
    && prev.value == next.value;
}
export default React.memo(FormInputPassword, areEqual);
