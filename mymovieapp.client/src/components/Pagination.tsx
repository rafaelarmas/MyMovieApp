import { useEffect, useState } from 'react';
import PageLink from './PageLink';
import './Pagination.css';
import { getPaginationItems } from '../lib/pagination';

export type Props = {
    currentPage: number;
    lastPage: number;
    maxLength: number;
    setCurrentPage: (page: number) => void;
};

export interface Movie {
    movieId: number;
    releaseDate: string;
    title: string;
    overview: string;
    popularity: string;
    voteCount: number;
    voteAverage: number;
    originalLanguage: string;
    genre: string;
    posterUrl: string;
}

export default function Pagination({
    currentPage,
    lastPage,
    maxLength,
    setCurrentPage,
}: Props) {
    const [movies, setMovies] = useState<Movie[]>();
    const pageNums = getPaginationItems(currentPage, lastPage, maxLength);
    const contents = movies === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th></th>
                    <th>Title</th>
                    <th>Release Date</th>
                    <th>Overview</th>
                    <th>Popularity</th>
                    <th>Vote Count</th>
                    <th>Vote Average</th>
                    <th>Original Language</th>
                    <th>Genre</th>
                </tr>
            </thead>
            <tbody>
                {movies.map(movie =>
                    <tr key={movie.movieId}>
                        <td><img src={movie.posterUrl} width='100px' /></td>
                        <td>{movie.title}</td>
                        <td>{movie.releaseDate.split("T")[0]}</td>
                        <td>{movie.overview}</td>
                        <td>{movie.popularity}</td>
                        <td>{movie.voteCount}</td>
                        <td>{movie.voteAverage}</td>
                        <td>{movie.originalLanguage}</td>
                        <td>{movie.genre}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    useEffect(() => {
        populateMovieData(1, '', 'None');
    }, []);

    return (
        <div className="container">
            <div>
                <label>Page Size:</label>
                <select className="pageSize" name="pageSize">
                    <option value="10">10</option>
                    <option value="25">25</option>
                    <option value="50">50</option>
                </select>
                <label>Genre:</label>
                <select className="genreFilter" name="genreFilter">
                    <option value="None">None</option>
                    <option value="Action">Action</option>
                    <option value="Adventure">Adventure</option>
                    <option value="Animation">Animation</option>
                    <option value="Comedy">Comedy</option>
                    <option value="Crime">Crime</option>
                    <option value="Documentary">Documentary</option>
                    <option value="Drama">Drama</option>
                    <option value="Family">Family</option>
                    <option value="Fantasy">Fantasy</option>
                    <option value="History">History</option>
                    <option value="Horror">Horror</option>
                    <option value="Music">Music</option>
                    <option value="Mystery">Mystery</option>
                    <option value="Romance">Romance</option>
                    <option value="Science Fiction">Science Fiction</option>
                    <option value="Thriller">Thriller</option>
                    <option value="TV Movie">TV Movie</option>
                    <option value="War">War</option>
                    <option value="Western">Western</option>
                </select>
                <br />
                <br />
                <label>Keyword:</label>
                <input type="text" name="keyword" className="keyword" />
                <input type="button" className="go" value="GO" onClick={() => pageClick(1)} />
            </div>
            <nav className="pagination" aria-label="Pagination">
                <PageLink
                    disabled={currentPage === 1}
                    onClick={() => pageClick(currentPage - 1)}
                >
                    Previous
                </PageLink>
                {pageNums.map((pageNum, idx) => (
                    <PageLink
                        key={idx}
                        active={currentPage === pageNum}
                        disabled={isNaN(pageNum)}
                        onClick={() => pageClick(pageNum)}
                    >
                        {!isNaN(pageNum) ? pageNum : '...'}
                    </PageLink>
                ))}
                <PageLink
                    disabled={currentPage === lastPage}
                    onClick={() => pageClick(currentPage + 1) }
                >
                    Next
                </PageLink>
            </nav>
        
            { contents }
        </div>
    );

    function pageClick(page: number) {
        setCurrentPage(page);
        const keyword = document.getElementsByClassName("keyword").keyword.value;
        maxLength = parseInt(document.getElementsByClassName("pageSize").pageSize.value);
        const seletedGenre = document.getElementsByClassName("genreFilter").genreFilter.value;
        populateMovieData(page, keyword, seletedGenre);
    }

    async function populateMovieData(page: number, keyword: string, seletedGenre: string) {
        const keywordText = keyword !== null && keyword !== undefined ? 'keyword=' + keyword + '&' : '';
        const genreText = seletedGenre === 'None' ? '' : '&genre=' + seletedGenre;
        const response = await fetch('http://localhost:5150/getmoviesbykeyword?' + keywordText + 'pageSize=' + maxLength + '&currentPage=' + page + genreText);
        if (response.ok) {
            const data = await response.json();
            setMovies(data);
        }
    }
}