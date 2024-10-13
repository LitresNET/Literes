import React from 'react';
import {getByTestId, render} from '@testing-library/react';
import { Banner } from '../../../src/components/UI/Banner/Banner.jsx';

describe('Banner component', () => {
    it('renders with shadow class when withshadow is true', () => {
        const { container } = render(<Banner withshadow={true} className="additional-class"></Banner>);
        const bannerDiv = container.firstChild;
        expect(bannerDiv).toHaveClass('banner banner-shadow additional-class');
    });

    it('renders without shadow class when withshadow is false', () => {
        const { container } = render(<Banner withshadow={false} className="additional-class"></Banner>);
        const bannerDiv = container.firstChild;
        expect(bannerDiv).toHaveClass('banner additional-class');
        expect(bannerDiv).not.toHaveClass('banner-shadow');
    });

    it('renders text correctly', () => {
        const { getByText } = render(<Banner>Content</Banner>);
        expect(getByText('Content')).toBeInTheDocument();
    });

    it('applies additional className correctly', () => {
        const { container } = render(<Banner className="extra-class"></Banner>);
        const bannerDiv = container.firstChild;
        expect(bannerDiv).toHaveClass('banner extra-class');
    });

    it('renders same children correctly', () => {
        const { getByTestId } = render(
            <Banner>
                <Banner data-testid={"same-child"}></Banner>
            </Banner>
        );
        expect(getByTestId('same-child')).toBeInTheDocument();
    });

    it('renders multiple children correctly', () => {
        const { getByText } = render(
            <Banner>
                <div>Child 1</div>
                <div>Child 2</div>
            </Banner>
        );
        expect(getByText('Child 1')).toBeInTheDocument();
        expect(getByText('Child 2')).toBeInTheDocument();
    });
});
