import React from 'react';
import { render } from '@testing-library/react';
import '@testing-library/jest-dom';
import { IconButton } from '../../../src/components/UI/Icon/IconButton/IconButton.jsx';

describe('IconButton component', () => {
    it('renders an anchor element with the correct href', () => {
        const href = 'https://example.com';
        const { getByTestId } = render(<IconButton href={href}/>);
        const anchorElement = getByTestId('icon-button-link');
        expect(anchorElement).toBeInTheDocument();
        expect(anchorElement).toHaveAttribute('href', href);
    });

    it('renders the Icon component with the correct props', () => {
        const path = 'icon-path';
        const { getByRole } = render(<IconButton path={path} size="mini" />);
        const imgElement = getByRole('img');
        expect(imgElement).toBeInTheDocument();
        expect(imgElement).toHaveAttribute('src', path);
        expect(imgElement).toHaveStyle({ width: '16px', height: '16px' });
    });
});
