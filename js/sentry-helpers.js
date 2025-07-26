// Global error handling for Blazor WebAssembly
window.addEventListener('error', function(event) {
    // Capture JavaScript errors and send them to Blazor
    const errorInfo = {
        message: event.message,
        filename: event.filename,
        lineno: event.lineno,
        colno: event.colno,
        stack: event.error ? event.error.stack : null,
        userAgent: navigator.userAgent,
        url: window.location.href,
        timestamp: new Date().toISOString()
    };
    
    // Try to send to Blazor if available
    if (window.DotNet && window.DotNet.invokeMethodAsync) {
        try {
            window.DotNet.invokeMethodAsync('TestArena', 'CaptureJavaScriptError', errorInfo);
        } catch (e) {
            console.error('Failed to send error to Blazor:', e);
        }
    }
    
    console.error('Captured JavaScript error:', errorInfo);
});

// Capture unhandled promise rejections
window.addEventListener('unhandledrejection', function(event) {
    const errorInfo = {
        message: event.reason ? event.reason.toString() : 'Unhandled promise rejection',
        type: 'unhandled_promise_rejection',
        reason: event.reason,
        userAgent: navigator.userAgent,
        url: window.location.href,
        timestamp: new Date().toISOString()
    };
    
    if (window.DotNet && window.DotNet.invokeMethodAsync) {
        try {
            window.DotNet.invokeMethodAsync('TestArena', 'CaptureJavaScriptError', errorInfo);
        } catch (e) {
            console.error('Failed to send promise rejection to Blazor:', e);
        }
    }
    
    console.error('Captured unhandled promise rejection:', errorInfo);
});

// Performance monitoring helpers
window.sentryHelpers = {
    // Track page navigation
    trackNavigation: function(pageName) {
        const navigationInfo = {
            page: pageName,
            url: window.location.href,
            timestamp: new Date().toISOString(),
            performance: {
                loadEventEnd: performance.timing.loadEventEnd,
                navigationStart: performance.timing.navigationStart,
                loadTime: performance.timing.loadEventEnd - performance.timing.navigationStart
            }
        };
        
        if (window.DotNet && window.DotNet.invokeMethodAsync) {
            try {
                window.DotNet.invokeMethodAsync('TestArena', 'TrackNavigation', navigationInfo);
            } catch (e) {
                console.error('Failed to track navigation:', e);
            }
        }
    },
    
    // Track user interactions
    trackInteraction: function(elementType, elementId, action) {
        const interactionInfo = {
            elementType: elementType,
            elementId: elementId,
            action: action,
            url: window.location.href,
            timestamp: new Date().toISOString()
        };
        
        if (window.DotNet && window.DotNet.invokeMethodAsync) {
            try {
                window.DotNet.invokeMethodAsync('TestArena', 'TrackUserInteraction', interactionInfo);
            } catch (e) {
                console.error('Failed to track interaction:', e);
            }
        }
    }
};
