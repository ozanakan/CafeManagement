import {
  MenuFoldOutlined, MenuUnfoldOutlined,
} from "@ant-design/icons";
import { Button, Menu } from "antd";
import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "./index.scss";
import LeftMenu from "./left-menu";
import Logo from "../../../assets/img/left-menu/logo.png";

const LeftSection = ({ onChangeCollapsed }) => {
  const navigate = useNavigate();
  const [selectedKey, setSelectedKey] = useState("/");
  const [openKey, setOpenKey] = useState();
  const [leftMenu, setLeftMenu] = useState(LeftMenu);

  const [collapsed, setCollapsed] = useState(
    localStorage.getItem("leftMenuCollapsed") == "true" ||
    window.innerWidth < 900
  );
  const toggleCollapsed = () => {
    localStorage.setItem("leftMenuCollapsed", !collapsed);
    setCollapsed(!collapsed);
  };
  useEffect(() => {
    onChangeCollapsed(collapsed);
  }, [collapsed]);

  const handleResize = useCallback(() => {
    if (window.innerWidth < 900) setCollapsed(true);
    else setCollapsed(false);
  }, []);

  const onClick = (e) => {
    navigate(`${e.key}`);
  };

  window.addEventListener("resize", handleResize);

  return (
    <div className="left-section" style={{
      width: collapsed ? 80 : 256
    }}>
      <div className={"logoContainer"} style={{ cursor: "pointer", paddingBottom: 20 }} onClick={() => document.location.href = "/#/dashboard/alpha"}>
        <div className={"logo"}>
          <img src={Logo} style={{ width: "35%" }} className="mr-2" alt="" />
        </div>
      </div>
      <div className="left-menu">
        {selectedKey && leftMenu && (
          <Menu
            defaultSelectedKeys={[selectedKey]}
            defaultOpenKeys={[openKey]}
            mode="inline"
            inlineCollapsed={collapsed}
            onClick={onClick}
          >
            {leftMenu?.map((menu) => {
              return (
                <>
                  {menu.children && menu.children.length > 0 ? (
                    <Menu.SubMenu
                      key={menu.key}
                      icon={menu.icon}
                      title={menu.label}
                    >
                      {menu.children && menu.children.map((subMenu) => {
                        if (subMenu.children && subMenu.children.length) {
                          return (
                            <Menu.SubMenu
                              key={subMenu.key}
                              title={subMenu.label}
                            >
                              {subMenu.children && subMenu.children.length > 0 && subMenu.children.map((menuItem) => {
                                return (
                                  <Menu.Item key={menuItem.key} icon={menuItem.icon}>
                                    <Link to={menuItem.key}>{(menuItem.label)}</Link>
                                  </Menu.Item>
                                );
                              })}
                            </Menu.SubMenu>
                          );
                        }
                        else {
                          return (
                            <Menu.Item className="menu-item" key={subMenu.key} icon={subMenu.icon}>
                              <Link to={subMenu.key}>{subMenu.label}</Link>
                            </Menu.Item>
                          );
                        }
                      })}
                    </Menu.SubMenu>
                  ) : (
                    <Menu.Item key={menu.key} icon={menu.icon}>
                      <Link to={menu.key}>{menu.label}</Link>
                    </Menu.Item>
                  )}
                </>
              );
            })}
          </Menu>
        )}
      </div>
      <div className="header" style={{ position: 'absolute', bottom: 1 }}>
        <Button className="collapse-button" onClick={toggleCollapsed}>
          {collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
        </Button>
      </div>
    </div>
  );
};
export default LeftSection;
