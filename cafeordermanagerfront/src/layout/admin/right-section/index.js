import './index.scss';

const RightSection = ({ collapsed, children }) => {
  return (
    <>
      <div className='right-section' style={{ width: collapsed ? "calc(100% - 80px)" : "calc(100% - 256px)" }}>

        <div className='content'>
          {children}
        </div>
      </div >
      <div>

      </div>
    </>
  );
};
export default RightSection;