import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { BookCard } from '../../src/components/BookCard/BookCard.jsx';
import { axiosToLitres } from '../../src/hooks/useAxios';
import { toast } from 'react-toastify';
import IMAGES from "../../src/assets/images.jsx";
import {MemoryRouter} from "react-router-dom";
import {addBookToFavourites} from "../../src/features/addBookToFavourites.js";

vi.mock('../../src/hooks/useAxios', () => ({
    axiosToLitres: {
        get: vi.fn(),
        post: vi.fn()
    },
}));

vi.mock('../../src/features/addBookToFavourites', () => ({
    addBookToFavourites: vi.fn(),
}));
vi.mock('react-toastify', () => ({
    toast: {
        error: vi.fn(),
    },
}));
    describe('BookCard Component', () => {
    const mockBookData = {
        name: 'Test Book',
        coverUrl: 'test-cover-url.jpg',
        price: '100',
    };

    beforeEach(() => {
        axiosToLitres.get.mockResolvedValue({ data: mockBookData });
    });

    afterEach(() => {
        vi.clearAllMocks();
    });

    it('renders book name and price correctly', async () => {
        render(
            <MemoryRouter>
                <BookCard bookId={1} />
            </MemoryRouter>);

        await waitFor(() => {
            expect(screen.getByText('Test Book')).toBeInTheDocument();
            expect(screen.getByText('100$')).toBeInTheDocument();
        });
    });

    it('renders default cover if coverUrl is not provided', async () => {
        axiosToLitres.get.mockResolvedValue({ data: { ...mockBookData, coverUrl: null } });

        render(
            <MemoryRouter>
                <BookCard bookId={1} />
            </MemoryRouter>);

        await waitFor(() => {
            const coverImage = screen.getByRole('img', {name: /book-cover/i});
            expect(coverImage).toHaveAttribute('src', IMAGES.default_cover); // Замените на ваш путь к изображению по умолчанию
        });
    });

    it('displays error toast if fetching book data fails', async () => {
        axiosToLitres.get.mockRejectedValue(new Error('Network Error'));

        render(
            <MemoryRouter>
                <BookCard bookId={1} />
            </MemoryRouter>
        );

        await waitFor(() => {
            expect(toast.error).toHaveBeenCalledWith('Book Card: Network Error', { toastId: 'BookCardError' });
        });
    });

    it('calls addBookToFavourites when bookmark button is clicked', async () => {
        render(
            <MemoryRouter>
                <BookCard bookId={1} />
            </MemoryRouter>);

        await waitFor(() => {
            const bookmarkButton = screen.getByRole('button', { name: /book-favourite/i });
            fireEvent.click(bookmarkButton);
            expect(addBookToFavourites).toHaveBeenCalledWith(1);
        });
    });
});
