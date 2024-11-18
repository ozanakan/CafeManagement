import './index.scss';

const Auth = ({ children }) => {
    return (
        <div className="auth-container">
            <div className='form-modal'>
                {children}
            </div>
        </div>
    );
};
export default Auth;