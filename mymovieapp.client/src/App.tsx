import { useState } from 'react';
import Pagination from './components/Pagination';
import './App.css';


function App() {
    
    const [currentPage, setCurrentPage] = useState(1);
    const lastPage = 10;

    return (
        <div className="container">
            <h1>Search</h1>
            <Pagination
                currentPage={currentPage}
                lastPage={lastPage}
                maxLength={10}
                setCurrentPage={setCurrentPage}
            />
        </div>
    );

}

export default App;