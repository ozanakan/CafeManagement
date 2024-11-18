import React, { useEffect, useState } from 'react'
import { Button } from 'antd';

const FormButton = ({
    icon,
    color = "black",
    fill = false,
    onClick,
    text,
    hideTextOnMobile,
    loading,
    size = "middle", // small, middle, large
    disabled = false,
    show = true,
    style

}) => {

    const [windowSize, setWindowSize] = useState([
        window.innerWidth,
        window.innerHeight,
    ]);

    useEffect(() => {
        const handleWindowResize = () => {
            setWindowSize([window.innerWidth, window.innerHeight]);
        };

        window.addEventListener('resize', handleWindowResize);

        return () => {
            window.removeEventListener('resize', handleWindowResize);
        };
    }, []);

    if (hideTextOnMobile && windowSize[0] && windowSize[0] < 700)
        text = "";

    const btnClassName = `btn btn-${color}${fill ? "-fill" : ""}${disabled ? " btn-disabled" : ""}`;

    const onClickButton = (e) => {
        e.preventDefault()
        if (onClick instanceof Function)
            onClick();
    }
    if (show)
        return (
            <>
                <Button
                    onClick={onClickButton}
                    type={fill ? "primary" : "link"}
                    disabled={disabled}
                    loading={loading}
                    icon={icon}
                    className={btnClassName}
                    size={size}
                    style={{ backgroundColor: color, display: "flex", alignItems: "center", ...style }}>
                    {text}
                </Button >
            </>
        )
    else
        return (<></>)
}

const areEqual = (prev, next) => {
    return prev.icon === next.icon
        && prev.color === next.color
        && prev.fill === next.fill
        && prev.onClick === next.onClick
        && prev.text === next.text
        && prev.hideTextOnMobile === next.hideTextOnMobile
        && prev.loading === next.loading
        && prev.size === next.size
        && prev.disabled === next.disabled
        && prev.show === next.show
}
export default React.memo(FormButton, areEqual);