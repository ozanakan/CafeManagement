import Admin from "../layout/admin";
import store from 'store';

const RouterHelper = ({ component }) => {
    const token = store.get('token')
    if (token)
        return <Admin>{component}</Admin>
    else {
        console.log("redirect")
        window.location.href = "/auth/login"
    }
}

export default RouterHelper;