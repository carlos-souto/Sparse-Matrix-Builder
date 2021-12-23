% reset workspace
clear all; close all; clc;

% add to path: parent_directoy
addpath([pwd, '\..\']);

% load the .NET assembly
dll = [pwd '\..\SparseLibrary.dll'];
assembly = NET.addAssembly(dll);

% matrix row/column lengths to test
lengths = 0:5e5:5e6;

% number of test repetitions
reps = 10;

% data for plotting
times_matlab = zeros(size(lengths));
times_custom = zeros(size(lengths));

% run loop (for each matrix row/column length)
for i = 2:length(lengths)
    
    % info
    fprintf('Run %i of %i (sparse matrix row/column length = %i)...\n', i-1, length(lengths)-1, lengths(i));
    
    % initialize timers
    time_matlab = 0.0;
    time_custom = 0.0;
    
    % test loop (for each test repetition)
    for j = 1:reps
        
        % matrix size
        m = lengths(i);
        n = lengths(i);
        
        % number of elements to get/set
        k = 5e3;
        
        % random row/col indexes and vals
        rows = randi([1, m], k, 1);
        cols = randi([1, n], k, 1);
        vals = rand(k, 1);
        
        % test and time matlab routine
        tic;
        matrix_matlab = test_matlab(m, n, rows, cols, vals);
        time_matlab = time_matlab + toc;
        
        % test and time custom routine
        tic;
        matrix_custom = test_custom(m, n, rows, cols, vals);
        time_custom = time_custom + toc;
        
        % check if matrices are equal
        if ~isequal(matrix_matlab, matrix_custom)
            error('Test Failed!');
        end
        
    end
    
    % average elapsed time in ms
    times_matlab(i) = 1000*time_matlab/reps;
    times_custom(i) = 1000*time_custom/reps;
    
end

% plot
figure(1); hold on; grid on; legend show;
xlabel('Sparse Matrix Row/Column Length');
ylabel(['Averaged elapsed time of ' num2str(reps) ' tests [ms]']);
title(['Time taken to initialize + set & get ' num2str(k) ' random elements + convert back to MATLAB sparse']);
plot(lengths(2:end), times_matlab(2:end), 'DisplayName', 'MATLAB Sparse Matrix');
plot(lengths(2:end), times_custom(2:end), 'DisplayName', 'C# Sparse Matrix Builder');

function matrix = test_matlab(m, n, rows, cols, vals)
    
    % create the matrix
    matrix = sparse(m, n);
    
    % set random values in the matrix
    for i = 1:1:length(vals)
        matrix(rows(i), cols(i)) = vals(i);
    end
    
    % get random values from the matrix
    for i = 1:1:length(vals)
        ignore = matrix(rows(i), cols(i));
    end
    
end

function matrix = test_custom(m, n, rows, cols, vals)
    
    % create the matrix
    matrix = SparseMatrixBuilder(m, n);
    
    % set random values in the matrix
    matrix.Set(rows, cols, vals);
    
    % get random values from the matrix
    ignore = matrix.Get(rows, cols);
    
    % convert to matlab sparse
    matrix = matrix.ToMatlabSparse();
    
end
