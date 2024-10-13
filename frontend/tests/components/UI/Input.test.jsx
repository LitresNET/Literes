import React from 'react';
import {render, fireEvent, getByTestId} from '@testing-library/react';
import '@testing-library/jest-dom';
import { Input } from '../../../src/components/UI/Input/Input.jsx';
import ICONS from '../../../src/assets/icons.jsx';

vi.mock('../../../src/components/UI/Icon/Icon', () => ({
    Icon: ({ path, onClick }) => <svg data-testid={`icon-${path}`} onClick={onClick} />,
}));

describe('Input component', () => {
    it('renders text input correctly', () => {
        const {getByPlaceholderText} =
            render(<Input type="text" id="text-input" placeholder="Enter text" />);
        const input = getByPlaceholderText('Enter text');

        expect(input).toBeInTheDocument();
        expect(input).toHaveAttribute('type', 'text');
    });


    it('renders number input with increment/decrement icons', () => {
        const { container, getByTestId } =
            render(<Input type="number" />);
        const inputElement = container.querySelector('input[type="number"]');

        expect(inputElement).toBeInTheDocument();
        expect(getByTestId(`icon-${ICONS.minus_circle}`)).toBeInTheDocument();
        expect(getByTestId(`icon-${ICONS.plus_circle}`)).toBeInTheDocument();
    });

    it('increments and decrements the number input value', () => {
        const { container, getByTestId } =
            render(<Input type="number" />);
        const inputElement = container.querySelector('input[type="number"]');
        const incrementIcon = getByTestId(`icon-${ICONS.plus_circle}`);
        const decrementIcon = getByTestId(`icon-${ICONS.minus_circle}`);

        expect(inputElement.value).toBe('1');

        fireEvent.click(incrementIcon);
        expect(inputElement.value).toBe('2');

        fireEvent.click(decrementIcon);
        expect(inputElement.value).toBe('1');
    });

    it('renders checkbox input correctly', () => {
        const iconPath = ICONS.money;
        const {getByRole, getByTestId}
            = render(<Input type="checkbox" id="checkbox-input" iconpath={iconPath} />);
        const input = getByRole('checkbox');
        expect(input).toBeInTheDocument();
        expect(input).toHaveAttribute('type', 'checkbox');

        const icon = getByTestId(`icon-${iconPath}`);
        expect(icon).toBeInTheDocument();
    })

    it('renders password input correctly', () => {
        const { container } = render(<Input type="password" />);
        const inputElement = container.querySelector('input[type="password"]');

        expect(inputElement).toBeInTheDocument();
    });

    it('does not set amount less than 1 or more than 99', () => {
        const {getByDisplayValue, getByTestId}
            = render(<Input type="number" id="number-input" />);
        const incrementIcon = getByTestId(`icon-${ICONS.plus_circle}`);
        const decrementIcon = getByTestId(`icon-${ICONS.minus_circle}`);

        fireEvent.click(decrementIcon);
        expect(getByDisplayValue('1')).toBeInTheDocument();

        for (let i = 0; i < 98; i++) {
            fireEvent.click(incrementIcon);
        }
        expect(getByDisplayValue('99')).toBeInTheDocument();
        fireEvent.click(incrementIcon);
        expect(getByDisplayValue('99')).toBeInTheDocument();
    });
});
