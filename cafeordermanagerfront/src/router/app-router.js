import Auth from "../layout/auth";
import Login from "../pages/auth/login";
import Orders from "../pages/orders/index";
import OrderForm from "../pages/orders/form";
import { Route, Routes } from "react-router-dom";
import RouterHelper from "./router-helper";

const AppRouter = () => {
    return (
        <Routes>
            <Route path="/auth/login" element={<Auth><Login /></Auth>} />
            <Route path="/orders" element={<RouterHelper component={<Orders />} />} />
            <Route path="/order/form" element={<RouterHelper component={<OrderForm />} />} />
            <Route path="/" element={<RouterHelper component={<Orders />} />} />
        </Routes>
    );
}

export default AppRouter;