import { Form, Input } from 'antd';
import TextArea from 'antd/lib/input/TextArea';
import React, { useEffect, useState } from 'react';
import general from '../../utils/general';
import useRefState from '../../utils/use-ref-state';
import FloatLabel from '../float-label';

const FormInput = ({
    onChange,
    onFocus,
    onBlur,
    onClick,
    onKeyPress,
    label,
    value,
    errorMessage,
    prefix,
    disabled,
    readOnly,
    className = "",
    size = "large",
    usePriceFormat = false,
    useNumberFormat = false,
    helperMessage,
    renderedValue
}) => {

    const [formattedValue, formattedValueRef, setFormattedValue] = useRefState(value);
    const [isLoaded, setIsLoaded] = useState(false);
    useEffect(() => {
        onFocusEvent()
        onBlurEvent()
    }, [renderedValue])

    useEffect(() => {
        if (!isLoaded && value || (formattedValueRef.current != value)) {
            if (usePriceFormat && value) {
                var val = value.toString().replaceAll(".", ",")
                //        setFormattedValue(general.formatPrice(val, true, CurrencyIcon[priceIcon]))
            } else if (useNumberFormat) {
                setFormattedValue(general.formatNumber(value));
            }
            else {
                setFormattedValue(value)
            }
            setIsLoaded(true)
        }
    }, [value, isLoaded])



    const onChangeEvent = (e) => {
        var val = e.target.value;


        if (onChange instanceof Function)
            onChange(val);
    }

    const onClickEvent = (e) => {
        if (onClick instanceof Function)
            onClick(e.target.value);
    }

    const onFocusEvent = () => {
        if (onFocus instanceof Function)
            onFocus();


    }

    const onBlurEvent = () => {
        if (onBlur instanceof Function)
            onBlur();


    }
    return (
        <Form.Item
            validateStatus={general.isNullOrEmpty(errorMessage) ? "" : "error"}
            help={errorMessage ? errorMessage : helperMessage}
        >
            <FloatLabel label={label} name={label} value={value} disabled={disabled} size={size}>
                <Input
                    autoComplete='new-password'
                    suffix={prefix}
                    allowClear
                    className={'border-style ' + className}
                    onKeyPress={onKeyPress}
                    value={formattedValue}
                    disabled={disabled}
                    readOnly={readOnly}
                    size={size}
                    onChange={onChangeEvent}
                    onClick={onClickEvent}
                    onFocus={onFocusEvent}
                    onBlur={onBlurEvent}
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
        && prev.prefix === next.prefix
        && prev.value === next.value
        && prev.renderedValue === next.renderedValue
        && prev.helperMessage === next.helperMessage
        && prev.priceIcon === next.priceIcon;
}
export default React.memo(FormInput, areEqual);
