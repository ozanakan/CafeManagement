import React, { useState } from "react";
import "./index.scss";

const FloatLabel = props => {
  const [focus, setFocus] = useState(false);
  const { children, label, value, disabled, focusInactive = false, size = "large" } = props;

  let labelClass = "label";
  if (disabled)
    labelClass += " label-float-disabled"
  if (value instanceof Array && value?.length > 0)
    labelClass += " label-float";
  else if (!(value instanceof Array) && (value || value === 0 || focus))
    labelClass += " label-float";

  return (
    <div
      className="float-label"
      style={{ display: 'flex', flexDirection: 'row' }}
      onBlur={() => setFocus(false)}
      onFocus={() => setFocus(!focusInactive && true)}
    >
      {children}
      <label className={`${labelClass} ${size === 'middle' ? 'label-middle' : ''}`}>{label}</label>
    </div>
  );
};

export default FloatLabel;
