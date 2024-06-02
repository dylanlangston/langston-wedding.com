import CssBaseline from '@mui/material/CssBaseline'; // Import CssBaseline
import WeddingDate from './wedding-information/WeddingDate';
import { weddingData } from './wedding-information/WeddingData';
import Header from './shared/Header'; 

function App() {
  return (
    <>
      <CssBaseline />
      <Header />
      <div>
        <WeddingDate weddingData={weddingData} />
      </div>
    </>
  );
}

export default App;