import './Loading.css';

const Loading = ({ size = 'medium', text = 'Carregando...' }) => {
  return (
    <div className={`loading loading-${size}`}>
      <div className="loading-spinner">
        <div className="spinner-ring"></div>
        <div className="spinner-ring"></div>
        <div className="spinner-ring"></div>
      </div>
      {text && <p className="loading-text">{text}</p>}
    </div>
  );
};

export default Loading;
