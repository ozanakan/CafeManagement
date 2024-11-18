import { Spin } from "antd";
import { useEffect, useState } from "react";
import { useDispatch } from 'react-redux';
import { Actions as ApiCallActions } from '../../redux/apiCall/reducers';
import { ServiceTypeEnum } from "../../utils/enums";

const Loader = ({ onLoaded, token }) => {
    const dispatch = useDispatch();
    const [userInfoLoaded, setUserInfoLoaded] = useState(false);

    useEffect(() => {
        if (!token || userInfoLoaded) {
            onLoaded()
        }
    }, [userInfoLoaded])


    //userInfo
    useEffect(() => {
        if (token)
            dispatch(ApiCallActions.Get({
                url: "security/UserInfo",
                serviceType: ServiceTypeEnum.User,
                showAlertOnError: true,
                onSuccess: async ({ data }) => {
                    global.userInfo = data.userInfo;
                    setUserInfoLoaded(true)
                }
            }))

    }, [])

    return (
        <>
            <Spin tip="" size="large" style={{ height: "100vh" }}>
                <div />
            </Spin>
        </>
    )
}

export default Loader;