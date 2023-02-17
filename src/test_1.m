% reset workspace
clear, clc, close all

% matrix sizes to test
vec_n = [1e4 1e5 1e6 5e6];

% number of elements to add to the matrix
n_els = 1e4;

% number of test repetitions
n_reps = 5;

% plot data
times_matlab = zeros(size(vec_n));
times_custom = zeros(size(vec_n));

% test loop (for each matrix size)
k = 0; % loop counter
for n = vec_n

    % info
    k = k + 1;
    fprintf("Run %i of %i (n = %i)...\n", k, length(vec_n), n);

    % initialize timers
    time_matlab = 0.0;
    time_custom = 0.0;

    % repetition loop
    for i = 1 : n_reps

        % generate random row/column indices and values
        rows = randi([1, n], n_els, 1);
        cols = randi([1, n], n_els, 1);
        vals = rand(n_els, 1);

        % test matlab
        tic;
        matrix_matlab = test_matlab(n, n, rows, cols, vals);
        time_matlab = time_matlab + toc;

        % test custom
        tic;
        matrix_custom = test_custom(n, n, rows, cols, vals);
        time_custom = time_custom + toc;

        % check if matrices are equal
        if ~isequal(matrix_matlab, matrix_custom)
            error("Test Failed!");
        end

    end

    % average elapsed time in ms
    times_matlab(k) = 1000*time_matlab/n_reps;
    times_custom(k) = 1000*time_custom/n_reps;
end

% plot
figure, hold on, box on, grid on, legend show
xlabel("Sparse matrix size, n")
ylabel(sprintf("Averaged elapsed time of %i runs [ms]", n_reps))
title(sprintf("Time taken to add %i entries to a n-by-n matrix", n_els));
plot(vec_n, times_matlab, 'DisplayName', 'MATLAB Sparse Matrix');
plot(vec_n, times_custom, 'DisplayName', 'Custom Sparse Matrix Builder');

function a = test_matlab(m, n, rows, cols, vals)
    a = sparse(m, n);
    for i = 1 : length(vals)
        a(rows(i), cols(i)) = a(rows(i), cols(i)) + vals(i);
    end
end

function a = test_custom(m, n, rows, cols, vals)
    a = SparseMatrixBuilder(m, n);
    for i = 1 : length(vals)
        a.add_val(rows(i), cols(i), vals(i));
    end
    a.squeeze();
    a = a.to_matlab();
end
