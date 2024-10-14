import React from 'react';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { Icon } from '../../../src/components/UI/Icon/Icon.jsx';

describe('Icon Component', () => {
    it('renders with default size', () => {
        render(<Icon path="test-path.png" />);
        const img = screen.getByRole('img');
        expect(img).toHaveStyle({ width: '25px', height: '25px' });
    });

    it('renders with mini size', () => {
        render(<Icon path="test-path.png" size="mini" />);
        const img = screen.getByRole('img');
        expect(img).toHaveStyle({ width: '16px', height: '16px' });
    });

    it('renders with custom size', () => {
        render(<Icon path="test-path.png" size="custom" width={30} />);
        const img = screen.getByRole('img');
        expect(img).toHaveStyle({ width: '30px', height: '30px' });
    });

    it('applies additional className', () => {
        render(<Icon path="test-path.png" className="additional-class" />);
        const div = screen.getByRole('img').parentElement;
        expect(div).toHaveClass('icon additional-class');
    });

    it('renders with correct src attribute', () => {
        render(<Icon path="test-path.png" />);
        const img = screen.getByRole('img');
        expect(img).toHaveAttribute('src', 'test-path.png');
    });
});
