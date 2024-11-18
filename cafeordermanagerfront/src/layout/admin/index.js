import './index.scss';
import LeftSection from "./left-section";
import RightSection from "./right-section";
import { useState, useCallback } from 'react'

const Admin = ({ children }) => {
    const [collapsed, setCollapsed] = useState(false);

    const onChangeCollapsed = useCallback((_collapsed) => {
        setCollapsed(_collapsed)
    })

    return (
        <div className="admin-container">
            <LeftSection onChangeCollapsed={onChangeCollapsed} />
            <RightSection collapsed={collapsed}>
                {children}

            </RightSection>
        </div>
    );
};
export default Admin;
