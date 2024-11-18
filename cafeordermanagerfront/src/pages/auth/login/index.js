import {
    LockOutlined,
    UserAddOutlined
} from "@ant-design/icons";
import FormButton from "../../../components/form/form-button";
import React, { useCallback, useState } from "react";
import { useDispatch } from "react-redux";
import { Actions as ApiCallActions } from "../../../redux/apiCall/reducers";
import store from "store";
import { ServiceTypeEnum } from "../../../utils/enums";
import useRefState from "../../../utils/use-ref-state";
import "./index.scss";
import FormInput from "../../../components/form/form-input";
import FormInputPassword from "../../../components/form/form-input-password";
import { useNavigate } from "react-router-dom";
import i18next from "i18next";
import Validator from '../../../components/validator'
import { message } from "antd";

const Login = ({ }) => {
    const navigate = useNavigate();
    const [validator, validatorRef,] = useRefState(new Validator());
    const validatorScopeKey = validator.generateNewScopeKey()
    const dispatch = useDispatch();
    const [loading, setLoading] = useState(false);
    const [formProps, formPropsRef, setFormProps] = useRefState({
        loginId: "Admin",
        password: "Test123"
    });
    const updateFormProps = (values) =>
        setFormProps((curr) => ({ ...curr, ...values }));
    const onChange = (key) => (value) => updateFormProps({ [key]: value });

    const onClickSave = useCallback(() => {
        const isValid = validator.allValid();
        if (isValid) {

            dispatch(
                ApiCallActions.Post({
                    url: "security/login",
                    serviceType: ServiceTypeEnum.Auth,
                    data: formPropsRef.current,
                    showAlertOnError: true,
                    showLoading: true,
                    onSuccess: async ({ data }) => {
                        console.log("data", data);
                        store.set("token", data.token);
                        window.location.href = "/";
                    },
                    onError: () => {
                        setLoading(false);
                    },
                    callback: () => setLoading(false),
                })
            );
        }
    }, [formProps]);


    return (
        <div className="login">
            <div className="right">
                <div className="title">{i18next.t("Login")}</div>

                <div className="mt-3">
                    <FormInput
                        label={i18next.t("auth.login.loginId")}
                        onChange={onChange("loginId")}
                        value={formProps?.loginId}
                        prefix={<UserAddOutlined />}
                        errorMessage={validator.register("loginId", formProps?.loginId, [{ rule: "required" }], validatorScopeKey)}
                    />
                </div>
                <div className="mt-3" >
                    <FormInputPassword
                        label={i18next.t("auth.password")}
                        onChange={onChange("password")}
                        value={formProps?.password}
                        errorMessage={validator.register("password", formProps?.password, [{ rule: "required" }], validatorScopeKey)}
                    />
                </div>
                <div style={{ marginTop: 20, width: '100%', justifyContent: 'center', display: 'flex' }}
                    className="button">
                    <FormButton
                        loading={loading}
                        text={i18next.t("auth.login")}
                        icon={<LockOutlined />}
                        fill={true}
                        color="blue"
                        onClick={onClickSave}
                    />
                </div>
            </div>
        </div >
    );
};
export default Login;
